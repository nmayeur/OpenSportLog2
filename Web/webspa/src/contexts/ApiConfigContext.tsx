import { createContext } from "react";
import { IApiConfig } from "../types/IApiConfig";

export const ApiConfigContext = createContext(
    { baseUrl: "https://osl-webapiapi-dev.azure-api.net/api", apiKey: "2d5915334aa74fb19fefe972c952c5d6" } as IApiConfig
);
