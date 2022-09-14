namespace PortfolioOptimization.Test 

open System
open System.IO
open System.Net
open System.Net.Http
open Xunit
open FsUnit
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection
open PortfolioOptimization
open DataAccess 

module DataAccessTests = 

    [<Fact>]
    let ``getAll should not return empty result``() = 
        let securities = [| "British American Tobacco Plc";"Absa Group Limited"|]
        let expected =  
                  [[ 699.3];
                   [182.58]]

        let actual = getSecuritiesData securities "2022-09-08"  "2022-09-10"
        actual |> List.map(fun lst ->  lst |> List.mapi(fun i value -> Math.Round(value,4))) |> should equal expected


