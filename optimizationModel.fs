namespace PortfolioOptimization

open System
open MathNet.Numerics
open MathNet.Numerics.Optimization
open MathNet.Numerics.LinearAlgebra
open PortfolioOptimization
open SecuritiesTypes
open DataAccess 


module OptimizationModels  = 

    let maximumSharpRatioPortfolio (securitiesList:Securities) (startDate:string) (endDate:string)  = 

        let assetReturns = getSecuritiesData securitiesList startDate endDate
        let securities = securitiesList.Security

        if not <| (List.map  List.length assetReturns |> List.pairwise |> List.forall (fun (a,b) -> a = b))  then
            printfn "Asset Returns List of Lists has lengths of %A - All Lenghts Should be equal" (List.map List.length assetReturns) 
            raise(ArgumentException("assetReturns List of Lists has varying lengths"))

        let n = List.length assetReturns

        (*
            Defining the objective Function
            - Minimizing for the sharp Ratio - Making the sharp ratio our objective function 
            - Define the variance helper function 
            - Define the Sharp ratio function
        *)
        
        let variance xs = 
            let d = float(List.length xs)
            let average = List.average xs
            let variance = List.sumBy (fun x -> pown (x - average)2) xs/d 
            sqrt(variance)

        let sharpRatio weights = 
            let portfolioReturns = List.map(List.average)assetReturns 
            let portfolioMean = List.zip weights portfolioReturns |> List.sumBy(fun (x,y) -> x * y)
            let portfolioSDev =  List.map(variance) assetReturns |> List.zip weights |> List.sumBy(fun (x,y) -> x * y)
            -(portfolioMean - 0.1)/portfolioSDev 

        (*
            Define the Optimization Engine using BFGS.
        *)

        let BfgsFindMinimum f startList =
            let convertedFunction =
                new System.Func<Vector<float>,float>(Vector.toList >> f)
            let startVector =
                startList
                |> List.toSeq
                |> Vector.Build.DenseOfEnumerable

            MathNet.Numerics.FindMinimum.OfFunction(convertedFunction,startVector, 1e-5, 10000)    
            |> Vector.toSeq 

        let initialWeights = List.replicate n (1.0/float n) 

        let srp = BfgsFindMinimum sharpRatio initialWeights
        let weights = Seq.map(fun x -> x/Seq.sum(srp))srp |> Seq.toList              
        let outPut = List.map2(fun x y ->  {Security = x ; weight = y}) (securities |> Array.toList) (weights |> List.map(fun x -> x * 100.0))
        outPut
        






