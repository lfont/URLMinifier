namespace URLMinifier.Minify

open URLMinifier
open Mono.Data.Sqlite

module MinifyCommand =

  let insertLongUrl (con: SqliteConnection) longUrl =
    use cmd = con.CreateCommand ()
    cmd.CommandText <- """INSERT INTO url_map (long_url)
                          VALUES (@longUrl);
                          SELECT last_insert_rowid();"""
    cmd.Parameters.AddWithValue("longUrl", longUrl) |> ignore
    cmd.ExecuteScalar() :?> int64

  let selectLongUrlId (con: SqliteConnection) longUrl =
      use cmd = con.CreateCommand ()
      cmd.CommandText <- """SELECT rowid FROM url_map
                            WHERE long_url = @longUrl;"""
      cmd.Parameters.AddWithValue("longUrl", longUrl) |> ignore
      let maybeRowId = cmd.ExecuteScalar()
      match maybeRowId with
        | null -> None
        | _ -> Some (maybeRowId :?> int64)

  let rec convertNumberToBase num buf bas =
    match num with
      | 0L -> buf
      | _  -> let r = int(num % int64(bas))
              let v = num / int64(bas)
              convertNumberToBase v (r :: buf) bas

  let mapIndicesToAlphabet alphabet indexer =
    indexer (String.length alphabet)
      |> Seq.map (fun e -> alphabet.[e])
      |> Array.ofSeq
      |> System.String.Concat

  let convertIdToAlphabet alphabet id =
    let convertId bas = convertNumberToBase id [] bas
    mapIndicesToAlphabet alphabet convertId

  let minifyUrl longUrl =
    use con = new SqliteConnection(Configuration.getDbConnectionString)
    con.Open ()
    let longUrlId = match selectLongUrlId con longUrl with
                      | Some id -> id
                      | None -> insertLongUrl con longUrl
    convertIdToAlphabet Configuration.getBase62Alphabet longUrlId
