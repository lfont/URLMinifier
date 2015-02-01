namespace URLMinifier.Minify

open System
open URLMinifier
open Nancy

type MinifyModule() as x =
  inherit NancyModule()

  let (?) (form: obj) param =
    (form :?> Nancy.DynamicDictionary).[param]

  do
    x.Get.["/"] <- fun _ -> x.View.["index"] :> obj

    x.Post.["/api/minify"] <- fun parameters ->
      let maybeUri = x.Request.Form?originalUrl.ToString()
      let res = match Uri.TryCreate(maybeUri, UriKind.Absolute) with
                  | true, uri -> let urlId = MinifyCommand.minifyUrl (uri.ToString())
                                 x.Response.AsText(sprintf "%s/%s" Configuration.getBaseUrl urlId)
                  | _ -> let r = new Response()
                         r.WithStatusCode(HttpStatusCode.BadRequest)
      res :> obj
