import { useEffect, useState } from "react";


interface IAthleteDto {
    id: number;
    name: string;
}
interface IActivityDto {
    id: number;
    name: string;
    athlete: IAthleteDto;
    location: string;
}

const useFetchActivityById = (baseUrl: string, api_key: string, activityId: number | null) => {
    console.log(`Calling useFetchActivityById for athleteId ${activityId}`)

    const [activity, setActivity] = useState<IActivityDto>({} as IActivityDto);
    const [loading, setloading] = useState(true);
    const [error, seterror] = useState("");

    useEffect(() => {
        if (activityId === null) {
            console.error("activityId is null")
            seterror("activityId is null")
            return
        }

        const fetchData = async () => {
            const url = `${baseUrl}/Activity/activityById?activityId=${activityId}`
            console.log(`URL : ${url}`)
            const requestHeaders: HeadersInit = new Headers();
            requestHeaders.set('Ocp-Apim-Subscription-Key', api_key);
            const data = await fetch(url, { headers: requestHeaders })
            const dto = await data.json() as IActivityDto
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
    }, [baseUrl, api_key, activityId]);

    return { activity, loading, error };
};

export default useFetchActivityById;