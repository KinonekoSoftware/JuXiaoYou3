
Task("Versioning")
    .Does(() =>
{
    var propsFile = "./Directory.Build.props";
    var readedVersion = XmlPeek(propsFile, "//Version");
    var currentVersion = new Version(readedVersion);
    
    var semVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build + 1);
    var version = semVersion.ToString();
    
    XmlPoke(propsFile, "//Version", version);
});
