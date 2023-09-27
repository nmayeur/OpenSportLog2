import { useEffect, useState } from "react";
import { ActivityListRowType } from "../types";

function _createData(
    id: number,
    name: string,
    athlete: string,
    location: string
): ActivityListRowType {
    return { id, name, athlete, location};
}

const _rows = [
    _createData(1, 'Test 1', 'Nico', 'Chevreuse'),
    _createData(2, 'Test 2', 'Nico', 'LCSC'),
    _createData(3, 'Test 3', 'Nico', 'Longchamp'),
    _createData(4, 'Test 4', 'Nico', 'Chevreuse'),
    _createData(5, 'Test 5', 'Nico', 'Thoiry'),
];

const useFetchActivities = (url: string) => {
    const [rows, setrows] = useState<ActivityListRowType[]>([]);
    const [loading, setloading] = useState(true);
    const [error, seterror] = useState("");

    useEffect(() => {
        seterror("")
        setrows(_rows)
        setloading(false)
    }, [url]);

    return { rows, loading, error };
};

export default useFetchActivities;