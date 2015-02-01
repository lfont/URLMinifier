namespace URLMinifier

open Owin
open Nancy
open Mono.Data.Sqlite

type Startup() =

  let createTable (con: SqliteConnection) =
    use cmd = con.CreateCommand ()
    cmd.CommandText <- """CREATE TABLE IF NOT EXISTS url_map
                          (long_url TEXT NOT NULL UNIQUE);"""
    cmd.ExecuteNonQuery () |> ignore

  let initializeDb =
    use con = new SqliteConnection(Configuration.getDbConnectionString)
    con.Open ()
    createTable con

  member x.Configuration(app: Owin.IAppBuilder) =
    initializeDb
    app.UseNancy() |> ignore
