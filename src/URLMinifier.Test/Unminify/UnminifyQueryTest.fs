namespace URLMinifier.Test.Unminify

open Fuchu
open URLMinifier.Unminify

module UnminifyQueryTest =

  let base62Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

  let testMapIdToIndices urlId expected =
    testCase (sprintf "%s to indices" urlId) <|
    fun _ -> Assert.Equal(sprintf "should equals %A" expected,
                          expected,
                          UnminifyQuery.mapIdToIndices base62Alphabet urlId)

  let testConvertIndicesToId idx expected =
    testCase (sprintf "%A to id" idx) <|
    fun _ -> Assert.Equal(sprintf "should equals %i" expected,
                          expected,
                          UnminifyQuery.convertIndicesToId 62 idx)

  [<Fuchu.Tests>]
  let tests =
    testList "LongQuery" [
      testList "mapIdToIndices" [
        testMapIdToIndices ""     [||]
        testMapIdToIndices "cb"   [|2; 1|]
        testMapIdToIndices "e9a"  [|4; 61; 0|]
        testMapIdToIndices "a7Qp" [|0; 59; 42; 15|]
      ];
      testList "convertIndicesToId" [
        testConvertIndicesToId [||]              0L
        testConvertIndicesToId [|2; 1|]          125L
        testConvertIndicesToId [|4; 61; 0|]      19158L
        testConvertIndicesToId [|0; 59; 42; 15|] 229415L
      ]
    ]
