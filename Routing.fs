module Routing

open Giraffe
open Microsoft.AspNetCore.Http
open PortfolioOptimization
open OptimizationModels 
open SecuritiesTypes 
open DataAccess

let getOptimizedSharpRaioPortfolio : HttpHandler   =
    fun (next : HttpFunc) (ctx : HttpContext) -> 
        task {
              let startDate = ctx.TryGetQueryStringValue "startDate"
                              |> Option.defaultValue "2018-01-01"

              let endDate = ctx.TryGetQueryStringValue "endDate"
                              |> Option.defaultValue "2022-05-22"

              let! securitiesList = ctx.BindJsonAsync<Securities>()                 
              let optimizedPortfolio = maximumSharpRatioPortfolio securitiesList startDate endDate
              return! json optimizedPortfolio next ctx
        }

let getInputData : HttpHandler   =
    fun (next : HttpFunc) (ctx : HttpContext) -> 
        task {
              let startDate = 
                   match ctx.TryGetQueryStringValue "startDate"  with 
                   |None -> "2018-01-01"
                   |Some  dateValue -> dateValue

              let endDate = 
                  match ctx.TryGetQueryStringValue "endDate" with 
                  | None -> "2022-05-22"
                  |Some dateValue -> dateValue 

              let! securitiesList = ctx.BindJsonAsync<Securities>()             
              let inputDataList = getInputData( Some securitiesList) startDate endDate
              return! json inputDataList next ctx
        }

let routes: HttpFunc -> HttpFunc =
    choose [
        GET >=>
            choose [
                route "/api/portfolioOptim" >=> getOptimizedSharpRaioPortfolio
                route "/api/portfolioOptimInputData" >=> getInputData             
                route "/" >=> text "hello"
            ]
         ]

