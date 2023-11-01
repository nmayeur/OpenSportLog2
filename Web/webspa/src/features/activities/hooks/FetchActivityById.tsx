import { useContext, useEffect, useState } from "react";
import { ApiConfigContext } from "../../../contexts/ApiConfigContext";


interface IAthleteDto {
    id: number;
    name: string;
}
interface IActivityDto {
    id: number;
    name: string;
    originId: string;
    originSystem: string;
    athlete: IAthleteDto;
    location: string;
    calories: number;
    temperature: number;
    sport: number;
    time: Date;
    timeSpanTicks: number;
    heartRate: number;
    cadence: number;
    power: number;
}

const useFetchActivityById = (activityId: number | null) => {
    console.log(`Calling useFetchActivityById for activityId ${activityId}`)

    const apiConfig = useContext(ApiConfigContext);
    const [activity, setActivity] = useState<IActivityDto>({} as IActivityDto);
    const [loading, setloading] = useState(true);
    const [error, seterror] = useState("");

    useEffect(() => {
        if (activityId === null) {
            console.debug("activityId is null")
            seterror("activityId is null")
            return
        }

        const fetchData = async () => {
            const url = `${apiConfig.baseUrl}/Activity/activityById?activityId=${activityId}`
            console.log(`URL : ${url}`)
            const requestHeaders: HeadersInit = new Headers();
            requestHeaders.set('Ocp-Apim-Subscription-Key', apiConfig.apiKey);
            const data = await fetch(url, { headers: requestHeaders })
            const dto = await data.json() as IActivityDto
            if (dto.time) {
                dto.time = new Date(dto.time)
            }
            setActivity(dto)
            seterror("")
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

    return { activity, loading, error };
};

export default useFetchActivityById;