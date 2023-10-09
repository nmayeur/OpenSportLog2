import { Paper } from "@mui/material";
import React from "react";
import useFetchActivitiesByAthlete from "../hooks/FetchActivitiesByAthlete";
import { DataGrid, GridColDef, GridRowSelectionModel } from "@mui/x-data-grid";

interface ActivitiesListProps {
    athleteId: number;
    onActivityIdChange: (activityId: number) => void
}

export const ActivitiesList = (props: ActivitiesListProps) => {

    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: 'Identifiant',
            minWidth: 100,
            align: 'right'
        },
        { field: 'name', headerName: 'Nom', minWidth: 170 },
        {
            field: 'athlete',
            headerName: 'Athlete',
            minWidth: 170
        },
        {
            field: 'location',
            headerName: 'Lieu',
            minWidth: 170
        }
    ];

    const [baseUrl] = React.useState(`https://osl-webapiapi-dev.azure-api.net/osl-dev/api`);
    const [api_key] = React.useState("2d5915334aa74fb19fefe972c952c5d6");

    const { rows, loading } = useFetchActivitiesByAthlete(baseUrl, api_key, props.athleteId);

    const handleSelection = (row: GridRowSelectionModel) => {
        const rowId = row[0]
        console.log(rowId)
        props.onActivityIdChange(rowId as number)
    }

    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }} elevation={3}>
            <DataGrid loading={loading} rows={rows} columns={columns} onRowSelectionModelChange={handleSelection} />
        </Paper>
    );
}