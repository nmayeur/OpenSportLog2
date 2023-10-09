import { useEffect, useState } from "react";
import { ActivityListRowType } from "../types";
import { IPaginatedItemsViewModel } from "../../../types/IPaginatedItemsViewModel";


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

const useFetchActivities = (url: string, api_key: string) => {
    console.log("Calling useFetchActivities")
    const [rows, setrows] = useState<ActivityListRowType[]>([]);
    const [loading, setloading] = useState(true);
    const [error, seterror] = useState("");

    useEffect(() => {
        const fetchData = async () => {
            console.log(`URL : ${url}`)
            const requestHeaders: HeadersInit = new Headers();
            requestHeaders.set('Ocp-Apim-Subscription-Key', api_key);
            const data = await fetch(url, { headers: requestHeaders })
            const _rows: IPaginatedItemsViewModel<IActivityDto> = await data.json() as IPaginatedItemsViewModel<IActivityDto>;

            seterror("")
            setrows(_rows.data.map(activity => { return { id: activity.id, name: activity.name, location: activity.location, athlete: activity.athlete.name } }))
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
    }, [url, api_key]);

    return { rows, loading, error };
};

export default useFetchActivities;