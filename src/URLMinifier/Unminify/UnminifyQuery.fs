namespace URLMinifier.Unminify

open URLMinifier
open Mono.Data.Sqlite

module UnminifyQuery =

  let selectLongUrlById (con: SqliteConnection) (id: int64) =
    use cmd = con.CreateCommand ()
    cmd.CommandText <- """SELECT long_url FROM url_map
                          WHERE rowid = @id;"""
    cmd.Parameters.AddWithValue("id", id) |> ignore
    let longUrl = cmd.ExecuteScalar()
    match longUrl with
      | null -> None
      | _    -> Some (longUrl.ToString())

  let mapIdToIndices (alphabet: string) (urlId: string) =
    urlId
    |> Seq.map (fun c -> alphabet.IndexOf c)
    |> Seq.toArray

  let convertIndicesToId bas indices =
    indices
      |> Array.fold (fun s i -> let exp = fst s
                                let acc = snd s + (int64 i) * int64 (pown bas exp)
                                (exp - 1, acc))
                    (Array.length indices - 1, 0L)
      |> snd

  let getLongIdFromUrlId alphabet urlId =
    let indices = mapIdToIndices alphabet urlId
    convertIndicesToId (String.length alphabet) indices

  let getUrl urlId =
    let id = getLongIdFromUrlId Configuration.getBase62Alphabet urlId
    use con = new SqliteConnection(Configuration.getDbConnectionString)
    con.Open ()
    selectLongUrlById con id
