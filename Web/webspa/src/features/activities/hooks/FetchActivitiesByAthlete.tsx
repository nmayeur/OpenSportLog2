import { useEffect, useState } from "react";
import { IPaginatedItemsViewModel } from "../../../types/IPaginatedItemsViewModel";
import { GridRowsProp } from "@mui/x-data-grid";


interface IAthleteDto {
    id: number;
    name: string;
}
interface IActivityDto {
    id: number;
    name: string;
    athlete: IAthleteDto;
    location: string;
    calories: number;
    temperature: number;
    sport: number;
    time: Date;
    timeSpanTicks: number;
}

const useFetchActivitiesByAthlete = (baseUrl: string, api_key: string, athleteId: number) => {
    console.log(`Calling useFetchActivitiesByAthlete for athleteId ${athleteId}`)
    const [rows, setrows] = useState<GridRowsProp>([] as GridRowsProp);
    const [loading, setloading] = useState(true);
    const [error, seterror] = useState("");

    useEffect(() => {
        const fetchData = async () => {
            const url = `${baseUrl}/Activity/activitiesByAthlete?athleteId=${athleteId}&pageSize=10&pageIndex=0`
            console.log(`URL : ${url}`)
            const requestHeaders: HeadersInit = new Headers();
            requestHeaders.set('Ocp-Apim-Subscription-Key', api_key);
            const data = await fetch(url, { headers: requestHeaders })
            const _rows: IPaginatedItemsViewModel<IActivityDto> = await data.json() as IPaginatedItemsViewModel<IActivityDto>;

            seterror("")
            setrows(_rows.data.map(activity => {
                return {
                    id: activity.id,
                    name: activity.name,
                    location: activity.location,
                    athlete: activity.athlete.name,
                    sport: activity.sport,
                    temperature: activity.temperature,
                    time: activity.time,
                    timespanTicks: activity.timeSpanTicks,
                    calories: activity.calories,
                }
            }))
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
    }, [baseUrl, api_key, athleteId]);

    return { rows, loading, error };
};

export default useFetchActivitiesByAthlete;