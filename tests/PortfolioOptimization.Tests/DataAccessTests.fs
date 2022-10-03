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
open SecuritiesTypes

module DataAccessTests = 
    
    [<Fact>]
    let ``getAll should not return empty result``() = 
        let inputData = 
            {
                startDate="2022-09-08"
                endDate="2022-09-10"
                Security = [| "British American Tobacco Plc";"Absa Group Limited"|]
            }      
        let expected =  
                  [[ 699.3];
                   [182.58]]

        let actual = getInputData inputData   //getSecuritiesData securities "2022-09-08"  "2022-09-10"
        actual.Prices |> Array.toList |> List.map(fun lst ->  lst |> List.mapi(fun i value -> Math.Round(value,2))) |> should equal expected


