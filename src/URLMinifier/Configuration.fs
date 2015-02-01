namespace URLMinifier

open System
open System.Configuration
open System.IO
open System.Reflection

module Configuration =

  let getAssemblyDirectory =
    let codeBase = Assembly.GetExecutingAssembly().CodeBase;
    let uri = new UriBuilder(codeBase);
    let path = Uri.UnescapeDataString(uri.Path);
    Path.GetDirectoryName(path);

  let getDbConnectionString =
    sprintf "Data Source=%s%curl-minifier.db, version=3"
            getAssemblyDirectory
            Path.DirectorySeparatorChar

  let getBase62Alphabet =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

  let getBaseUrl =
    ConfigurationManager.AppSettings.["baseUrl"]
