// include Fake lib
#r "../packages/FAKE/tools/FakeLib.dll"

#r "System.Core.dll"
#r "System.Xml.Linq.dll"

open System.Xml.Linq
open System.IO
open System.Xml
open Fake
open Fake.ReleaseNotesHelper

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


let buildProject proj =
    let outputDir = proj |> getOutputDirName
    MSBuildDebug outputDir "Build" [proj] |> ignore


let getFrameworkReferencesFromCsproj (csprojContent: XDocument) =
    csprojContent.Root.Descendants() 
        |> Seq.filter (fun el -> el.Name.LocalName = "Reference" && not el.HasElements)
        |> Seq.map (fun el -> el.Attribute(XName.Get("Include")))
        |> Seq.map (fun el -> { AssemblyName = el.Value; FrameworkVersions = [ "4.5" ] } )
        |> Seq.toList


let getProjectReferencesFromCsproj (csprojContent: XDocument) =
    csprojContent.Root.Descendants() 
        |> Seq.filter (fun el -> el.Name.LocalName = "ProjectReference")
        |> Seq.map (fun el -> el.Element(XName.Get("Name", "http://schemas.microsoft.com/developer/msbuild/2003")))
        |> Seq.map (fun el -> (el.Value, releaseNotes.NugetVersion))
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


let createPackage (buildDir: DirectoryInfo) =
    let packageName = buildDir.Name
    let packagePath = Path.Combine(artifactsPackDir, packageName)
    printfn "%s" packagePath
    trace ("Current package: " @@ packageName)

    trace "Copying files..."
    CopyDir packagePath buildDir.FullName (fun filePath -> Path.GetFileName(filePath).StartsWith(packageName))

    let dependencies, frameworkAssemblies = getSourceInfo packageName

    trace "Starting NuGet packaging..."
    trace packagePath

    NuGet (fun app -> 
        { app with
            NoPackageAnalysis = true
            Authors = ["dwdkls"]
            Project = packageName
            Description = "Pizza description"                               
            Summary = "Pizza summary"  
            ReleaseNotes = releaseNotes.Notes |> toLines
            Version = releaseNotes.NugetVersion
            Publish = false
            OutputPath = nupkgOutDir
            WorkingDir = packagePath
            FrameworkAssemblies = frameworkAssemblies
            Files = [ ( @"*.*", Some @"lib\net45", None ) ]
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
   ==> "BuildAll"
   ==> "CreatePackages"
   ==> "Default"

// start build
RunTargetOrDefault "Default"