{ dotnetCorePackages
, buildDotnetModule
}:

buildDotnetModule rec {
  pname = "opentabletdriver-web";
  name = pname;
  version = "1.0.0.0";

  src = ./.;

  dotnet-sdk = dotnetCorePackages.sdk_7;
  dotnet-runtime = dotnetCorePackages.aspnetcore_7;

  dotnetInstallFlags = [ "--framework=net7.0" ];

  nugetDeps = ./deps.nix;

  executables = [ "OpenTabletDriver.Web" ];
  projectFile = executables;
}
