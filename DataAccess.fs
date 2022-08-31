namespace PortfolioOptimization

open System
open System.IO
open FSharp.Data
open SecuritiesTypes

module DataAccess = 

    let filePath = System.IO.Path.GetFullPath("JohannesburgStockExchangeData.csv")            
    let equitiesPriceData = CsvFile.Load(filePath)    

    let getSecurities (startDate:string) (endDate:string) =
        equitiesPriceData.Rows
        |> Seq.filter(fun row -> row.["MarketDate"].AsDateTime() >= (DateTime.Parse(startDate)) && row.["MarketDate"].AsDateTime() < (DateTime.Parse(endDate))) 
        |> Seq.map(fun rows -> rows.["Name"])
        |> Seq.distinct
        |> Seq.toArray

    let getSecuritiesData (securitiesList:Securities) (startDate:string) (endDate:string) = 

        let securities = securitiesList.Security

        let getSecurityData index  = 
            equitiesPriceData.Rows
            |> Seq.filter(fun row -> row.["Name"]  = securities.[index])   
            |> Seq.filter(fun row -> row.["MarketDate"].AsDateTime() >= (DateTime.Parse(startDate)) && row.["MarketDate"].AsDateTime() < (DateTime.Parse(endDate)))     
            |> Seq.map(fun row -> row.["MarketPrice"].AsFloat())
            |> Seq.toList

        let priecesData =  [for i in 0 .. securities.Length-1 do getSecurityData i]
       
        let returnsFunction inputPrices = 
            let pricesToSeq =  List.toSeq inputPrices
            pricesToSeq
            |> Seq.skip 1 |> Seq.zip pricesToSeq
            |> Seq.map (fun (a, b) ->  (b/a) - 1.0)
            |> Seq.toList

        let assetReturns = List.map(returnsFunction)priecesData
        assetReturns

    let getInputData (securitiesList:Securities option) (startDate:string) (endDate:string)  = 

        let securities = 
            match securitiesList with 
            |Some securitiesList -> securitiesList.Security
            |None -> getSecurities startDate endDate

        let getInputData index  = 
            equitiesPriceData.Rows
            |> Seq.filter(fun row -> row.["Name"]  = securities.[index])   
            |> Seq.filter(fun row -> row.["MarketDate"].AsDateTime() >= (DateTime.Parse(startDate)) && row.["MarketDate"].AsDateTime() < (DateTime.Parse(endDate)))     
            |> Seq.map(fun row -> {Value = row.["MarketPrice"].AsDecimal() ;Date = row.["MarketDate"].AsDateTime()})
            |> Seq.toArray 

        let InputData =  [for i in 0 .. securities.Length-1 do {Name = securities.[i] ;Prices =  getInputData i}]
        InputData



