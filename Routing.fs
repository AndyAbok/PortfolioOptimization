module Routing

open Giraffe
open Microsoft.AspNetCore.Http
open PortfolioOptimization

let viewOptimizedResults (next: HttpFunc) (ctx : HttpContext) =    
    let optimizedPortfolio = OptimizationModel.meanVariancePortfolio
    json optimizedPortfolio next ctx

let routes: HttpFunc -> HttpFunc =
    choose [
        GET >=> 
            choose [
                route "/api/portfolioOptim" >=> viewOptimizedResults                
            ]       
        setStatusCode 404 >=> text "Not Found" ]
    

