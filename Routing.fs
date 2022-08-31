module Routing

open Giraffe
open Microsoft.AspNetCore.Http
open PortfolioOptimization
open OptimizationModel 
open SecuritiesTypes 


let viewOptimizedResults : HttpHandler   =
    fun (next : HttpFunc) (ctx : HttpContext) -> 
        task {
              let! modelparams = ctx.BindJsonAsync<Securities>()                 
              let optimizedPortfolio = meanVariancePortfolio modelparams
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
