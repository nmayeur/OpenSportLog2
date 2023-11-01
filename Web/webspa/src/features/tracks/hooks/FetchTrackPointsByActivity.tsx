import { useContext, useEffect, useState } from "react";
import { ApiConfigContext } from "../../../contexts/ApiConfigContext";


export interface ITrackPointDto {
    id: number;
    latitude: number;
    longitude: number;
    time: Date;
}

const useFetchTrackPointsByActivity = (activityId: number | null) => {
    console.log(`Calling useFetchTrackPointsByActivity for activityId ${activityId}`)
    const apiConfig = useContext(ApiConfigContext);
    const [rows, setrows] = useState<ITrackPointDto[]>([] as ITrackPointDto[]);
    const [loading, setloading] = useState(true);
    const [error, seterror] = useState("");

    useEffect(() => {
        const fetchData = async () => {
            if (!activityId) {
                return
            }

            const url = `${apiConfig.baseUrl}/Track/trackPointsByActivityId?activityId=${activityId}`
            console.log(`URL : ${url}`)
            const requestHeaders: HeadersInit = new Headers();
            requestHeaders.set('Ocp-Apim-Subscription-Key', apiConfig.apiKey);
            const data = await fetch(url, { headers: requestHeaders })
            const _rows: ITrackPointDto[] = await data.json() as ITrackPointDto[];

            seterror("")
            setrows(_rows)
            setloading(false)
        }
        fetchData().catch(error => {
            if (error instanceof TypeError) {
                console.error(error.message)
                seterror(error.message)
            } else if (error instanceof SyntaxError) {
                console.error(error.message)
                seterror(error.message)
            }

        });
    }, [apiConfig, activityId]);

    return { rows, loading, error };
};

export default useFetchTrackPointsByActivity;