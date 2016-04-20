// include Fake lib
#r "../packages/FAKE/tools/FakeLib.dll"

#r "System.Core.dll"
#r "System.Xml.Linq.dll"

open System
open System.Linq
open System.Xml.Linq
open System.IO
open System.Xml
open System.Text.RegularExpressions
open Fake
open Fake.ReleaseNotesHelper
open Fake.AssemblyInfoFile


// Properties
let artifactsBuildDir = @"..\.artifacts\build\"
let artifactsPackDir = @"..\.artifacts\pack\"
let srcRootDir = @"..\framework\"

let nuspecFile = ".\Pizza.nuspec" 
let releaseNotesPath = @"..\ReleaseNotes.md"

let nupkgOutDir = @"C:\LocalNuget"

let releaseNotes = LoadReleaseNotes releaseNotesPath

// Helpers
let getOutputDirName proj =
    let folderName = Directory.GetParent(proj).Name
    sprintf "%s%s/" artifactsBuildDir folderName

let readFrameworkAssemblyVersion () =
    GetAttributeValue "AssemblyVersion" (srcRootDir @@ @"Pizza.Framework\Properties\AssemblyInfo.cs")
        |> fun f -> f.Value.Trim('"')
        |> Version.Parse 

let buildPackageVersion (version: Version) =
    sprintf "%i.%i.%i-alpha%04i" version.Major version.Minor version.Build version.Revision

let isWebProject packageName =
    packageName = "Pizza.Mvc"

let getFrameworkReferencesFromCsproj (csprojContent: XDocument) =
    csprojContent.Root.Descendants() 
        |> Seq.filter (fun el -> el.Name.LocalName = "Reference" && not el.HasElements)
        |> Seq.map (fun el -> el.Attribute(XName.Get("Include")))
        |> Seq.map (fun el -> { AssemblyName = el.Value; FrameworkVersions = [ "4.5" ] } )
        |> Seq.toList


let getProjectReferencesFromCsproj (csprojContent: XDocument) =
    let packageVersion = buildPackageVersion(readFrameworkAssemblyVersion())
    csprojContent.Root.Descendants() 
        |> Seq.filter (fun el -> el.Name.LocalName = "ProjectReference")
        |> Seq.map (fun el -> el.Element(XName.Get("Name", "http://schemas.microsoft.com/developer/msbuild/2003")))
        |> Seq.map (fun el -> (el.Value, packageVersion))
        |> Seq.toList


let getSourceInfo packageName = 
    let srcPath = Path.Combine(srcRootDir, packageName)

    let csprojFile = sprintf "%s\%s.csproj" srcPath packageName
    trace ("Reading dependencies from csproj file: " @@ csprojFile)
    let csprojContent = XDocument.Load(csprojFile)
    let frameworkAssemblies = getFrameworkReferencesFromCsproj csprojContent
    let projectDependencies = getProjectReferencesFromCsproj csprojContent
   
    let packagesConfig = Path.Combine(srcPath, "packages.config") 
    trace ("Reading dependencies from packages.config: " @@ packagesConfig)
    let dependencies =
        if TestFile packagesConfig then getDependencies packagesConfig
        else []
    
    (dependencies @ projectDependencies, frameworkAssemblies)

let configurePackageFiles packageName buildDirPath = 
        let compiledFiles = [ ( @"*.*", Some @"lib\net45", None ) ]
        if isWebProject packageName then 
            let resourceFileName = sprintf "%s.%s" packageName "resources.dll"
            let buildDirInfo = new DirectoryInfo(buildDirPath)
            let resourceFiles = 
                buildDirInfo.GetDirectories()
                |> Seq.filter (fun di -> di.GetFiles(resourceFileName).Length = 1)
                |> Seq.map (fun di -> ( sprintf @"%s\%s" di.Name resourceFileName, Some (sprintf @"%s\%s" @"lib\net45\" di.Name), None )) 
                |> Seq.toList
            compiledFiles @ resourceFiles
        else
            compiledFiles

// Build Step Functions

let buildProject proj =
    let outputDir = proj |> getOutputDirName
    MSBuildDebug outputDir "Build" [proj] 
        |> Log "AppBuild-Output: "

let createPackage (buildDir: DirectoryInfo) =
    let packageName = buildDir.Name
    let packagePath = Path.Combine(artifactsPackDir, packageName)
    printfn "%s" packagePath
    trace ("Current package: " @@ packageName)

    trace "Copying current assembly files..."
    CopyDir packagePath buildDir.FullName (fun filePath -> Path.GetFileName(filePath).StartsWith(packageName))

    let dependencies, frameworkAssemblies = getSourceInfo packageName

    trace "Starting NuGet packaging..."
    trace packagePath

    let packageVersion = buildPackageVersion(readFrameworkAssemblyVersion())
    let files = configurePackageFiles packageName buildDir.FullName

    NuGet (fun app -> 
        { app with
            NoPackageAnalysis = true
            Authors = ["dwdkls"]
            Project = packageName
            Description = "Pizza description"                               
            Summary = "Pizza summary"  
            ReleaseNotes = releaseNotes.Notes |> toLines
            Version = packageVersion
            Publish = false
            OutputPath = nupkgOutDir
            WorkingDir = packagePath
            FrameworkAssemblies = frameworkAssemblies
            Files = files
            Dependencies = dependencies
        }) nuspecFile 


// Targets
Target "Clean" (fun _ ->
   trace "Cleanup..."
   CleanDirs [artifactsBuildDir; artifactsPackDir]
)


Target "RestorePackages" (fun _ -> 
    "../Pizza.sln"
    |> RestoreMSSolutionPackages (fun p ->
         { p with
             OutputPath = "../packages"
             Retries = 3 })
 )

Target "UpgradeAssemblyInfos" (fun _ ->
    trace "Upgrading AssemblyInfo information"

    let version = readFrameworkAssemblyVersion()
    let currentBuildNumber =
        match version.Revision with
        | -1 -> 1
        | _ -> version.Revision + 1

    let currentVersion = sprintf "%s.%d" releaseNotes.NugetVersion currentBuildNumber

    let assemblyInfos = !!(srcRootDir @@ "**\AssemblyInfo.cs") 
    ReplaceAssemblyInfoVersionsBulk assemblyInfos (fun f -> 
        { f with
                AssemblyVersion = currentVersion
                AssemblyFileVersion = currentVersion
        })      
 )
 
Target "BuildAll" (fun _ ->
    trace "Building framework projects..."
    !! "../framework/**/*.csproj"
        |> Seq.iter buildProject
)


Target "CreatePackages" (fun _ ->
    trace "Creating packages..."
    subDirectories (new DirectoryInfo(artifactsBuildDir))
        |> Seq.iter createPackage
)


Target "Default" (fun _ ->
    trace "Default Target invoked."
)

// Dependencies
"Clean"
   ==> "RestorePackages"
   ==> "UpgradeAssemblyInfos"
   ==> "BuildAll"
   ==> "CreatePackages"
   ==> "Default"

// start build
RunTargetOrDefault "Default"