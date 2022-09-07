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
              let! securitiesList = ctx.BindJsonAsync<Securities>()                 
              let optimizedPortfolio = maximumSharpRatioPortfolio securitiesList 
              return! json optimizedPortfolio next ctx
        }

let getInputData : HttpHandler   =
    fun (next : HttpFunc) (ctx : HttpContext) -> 
        task {                      
              let! input = ctx.BindJsonAsync<Securities>()  
              let withSecurities =
                    if Array.isEmpty input.Security then
                        { input with Securities.Security = getSecurities input.startDate input.endDate }
                    else
                        input        
              let inputDataList = getInputData withSecurities  
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

