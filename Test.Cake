    var target = Argument("target", "Clean");
    var configuration = Argument("configuration", "Release");
    
    //////////////////////////////////////////////////////////////////////
    // TASKS
    //////////////////////////////////////////////////////////////////////
    
    Task("Clean")
        .Does(() =>
    {        
        //
        // Versioning
        var propsFile = "JuXiaoYou-Version.props";
        Information(propsFile);
        var readedVersion = XmlPeek(propsFile, "//AssemblyVersion");
        var currentVersion = new Version(readedVersion);
        
        var semVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build + 1);
        var version = semVersion.ToString();
        Information(version);
        XmlPoke(propsFile, "//AssemblyVersion", version);
        XmlPoke(propsFile, "//FileVersion", version);
        XmlPoke(propsFile, "//InformationalVersion", version);
        
    });
    
    //////////////////////////////////////////////////////////////////////
    // EXECUTION
    //////////////////////////////////////////////////////////////////////
    
    RunTarget(target);