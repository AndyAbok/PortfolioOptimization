module SecuritiesTypes

type SecurityPrice =
    { Value : decimal
      Date : System.DateTime 
    }

type Security =
    { Name : string
      Prices : SecurityPrice array
    }

type OptimizedWeights =
    { Security : string
      weight : float
    }

type Securities =
    {
        startDate:string
        endDate:string
        Security : string array  
    }


