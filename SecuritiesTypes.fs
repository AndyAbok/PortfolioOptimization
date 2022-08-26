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
    { Name : string
      weight : float
    }


