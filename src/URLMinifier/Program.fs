namespace URLMinifier

open System
open Microsoft.Owin.Hosting
open URLMinifier

module Program =
  [<EntryPoint>]
  let main args =
    let url = "http://+:8080";
    WebApp.Start<Startup>(url) |> ignore
    printfn "Running on %s" url
    printfn "Press enter to exit"
    Console.ReadLine () |> ignore
    0
