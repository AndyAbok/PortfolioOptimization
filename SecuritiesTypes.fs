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
        Security : string array  
    }


