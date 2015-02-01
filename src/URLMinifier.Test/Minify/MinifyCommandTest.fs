namespace URLMinifier.Test.Minify

open Fuchu
open URLMinifier.Minify

module MinifyCommandTest =

  let testConvertNumberToBase62 num expected =
    testCase (sprintf "%i to base62" num) <|
    fun _ -> Assert.Equal(sprintf "should equals %A" expected,
                          expected,
                          MinifyCommand.convertNumberToBase num [] 62)

  let base62Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

  let testMapIndicesToAlphabet idx expected =
    testCase (sprintf "%A to alphabet" idx) <|
    fun _ -> Assert.Equal(sprintf "should equals %s" expected,
                          expected,
                          MinifyCommand.mapIndicesToAlphabet base62Alphabet (fun _ -> idx))

  [<Fuchu.Tests>]
  let tests =
    testList "MinifyCommand" [
      testList "convertNumberToBase" [
        testConvertNumberToBase62 0L      [ ]
        testConvertNumberToBase62 125L    [ 2; 1 ]
        testConvertNumberToBase62 34902L  [ 9; 4; 58 ]
        testConvertNumberToBase62 490535L [ 2; 3; 37; 53 ]
      ];
      testList "mapIndicesToAlphabet" [
        testMapIndicesToAlphabet [ ]              ""
        testMapIndicesToAlphabet [ 2; 1 ]         "cb"
        testMapIndicesToAlphabet [ 9; 4; 58 ]     "je6"
        testMapIndicesToAlphabet [ 2; 3; 37; 53 ] "cdL1"
      ]
    ]
