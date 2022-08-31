module Routing

open Giraffe
open Microsoft.AspNetCore.Http
open PortfolioOptimization
open OptimizationModel 
open SecuritiesTypes 

let viewOptimizedResults : HttpHandler   =
    fun (next : HttpFunc) (ctx : HttpContext) -> 
        task {
              let startDate = ctx.TryGetQueryStringValue "startDate"
                              |> Option.defaultValue "2018-01-01"

              let endDate = ctx.TryGetQueryStringValue "endDate"
                              |> Option.defaultValue "2022-05-22"

              let! modelparams = ctx.BindJsonAsync<Securities>()                 
              let optimizedPortfolio = meanVariancePortfolio modelparams startDate endDate
              return! json optimizedPortfolio next ctx
        }

let routes: HttpFunc -> HttpFunc =
    choose [
        GET >=>
            choose [
                route "/api/portfolioOptim" >=> viewOptimizedResults
                route "/" >=> text "hello"
            ]
         ]
