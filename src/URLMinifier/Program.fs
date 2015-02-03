namespace URLMinifier

open System
open System.Threading
open Microsoft.Owin.Hosting
open URLMinifier

module Program =
  [<EntryPoint>]
  let main args =
    let url = "http://+:8080";
    WebApp.Start<Startup>(url) |> ignore
    printfn "Running on %s" url

    // Under mono if you daemonize a process a Console.ReadLine will cause an EOF
    // so we need to block another way
    if args |> Array.exists (fun s ->
                             s.Equals("-d",
                                      StringComparison.CurrentCultureIgnoreCase))
    then
      Thread.Sleep Timeout.Infinite
      0
    else
      printfn "Press enter to exit"
      Console.ReadLine() |> ignore
      0
