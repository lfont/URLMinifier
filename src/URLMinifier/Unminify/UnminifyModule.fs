namespace URLMinifier.Unminify

open Nancy
open Nancy.Responses

type UnminifyModule() as x =
  inherit NancyModule()

  let (?) (parameters: obj) param =
    (parameters :?> Nancy.DynamicDictionary).[param]

  do
    x.Get.["/{urlId}"] <- fun parameters ->
      let res = match UnminifyQuery.getUrl (parameters?urlId.ToString()) with
                  | Some longUrl -> x.Response.AsRedirect(longUrl, RedirectResponse.RedirectType.Permanent)
                  | _ -> let r = new Response()
                         r.WithStatusCode(HttpStatusCode.NotFound)
      res :> obj
