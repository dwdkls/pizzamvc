## New in 0.2 (Released 2016/02/12)
* UserContext is no longer mocked, current logged user Id is really used for database audit.
* Additional user information can be stored in cookies and retrieved in frontend or backend side (as in sample project).
* Default implementations provides only Id (in addition to standard ASP.NET username) value in custom Principal model.
* Default security related actions (login, change password, logoff) moved to framework (controller, service).
* ASP.NET MembershipProvider is NO longer used.
* Pizza.Contracts splitted into two projects: Pizza.Contracts.Persistence and Pizza.Contracts.Presentation.
* General cleanup in references and NuGet packages.