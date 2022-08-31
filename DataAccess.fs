namespace PortfolioOptimization

open System
open System.IO
open FSharp.Data
open SecuritiesTypes

module SecuritiesAccess = 

    // let getSecurities = 
    //     [|"Safaricom Ltd Ord 0.05";"Kenya Commercial Bank Ltd Ord 1.00";
    //     "East African Breweries Ltd Ord 2.00";"Equity Bank Ltd Ord 0.50";
    //     "Kenya Power Lighting Co Ltd Ord 20.00" ;"Co-operative Bank of Kenya Ltd Ord 1.00"
    //     |]
    // let getSecurities = 
    //     [|"Accelerate Property Fund Limited"; "Deneb Investments Ltd";"British American Tobacco Plc";"Absa Group Limited";
    //     "Dipula Income Fund B";"African Dawn Capital Ltd";"Capitec Bank Holdings Limited"|]

    let getSecuritiesData (securitiesList:Securities) (startDate:string) (endDate:string) = 

        let filePath = System.IO.Path.GetFullPath("JohannesburgStockExchangeData.csv")            
        let equitiesPriceData = CsvFile.Load(filePath) 

        let securities = securitiesList.Name

        let getSecurityData index  = 
            equitiesPriceData.Rows
            |> Seq.filter(fun row -> row.["Name"]  = securities.[index])   
            |> Seq.filter(fun row -> row.["MarketDate"].AsDateTime() >= (DateTime.Parse(startDate)) && row.["MarketDate"].AsDateTime() < (DateTime.Parse(endDate)))     
            |> Seq.map(fun row -> row.["MarketPrice"].AsFloat())
            |> Seq.toList

        let priecesData =  [for i in 0 .. securities.Length-1 do getSecurityData i]
        //List.map(List.length)priecesData 

        let returnsFunction inputPrices = 
            let pricesToSeq =  List.toSeq inputPrices
            pricesToSeq
            |> Seq.skip 1 |> Seq.zip pricesToSeq
            |> Seq.map (fun (a, b) ->  (b/a) - 1.0)
            |> Seq.toList

        let assetReturns = List.map(returnsFunction)priecesData
        assetReturns






