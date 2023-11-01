import { Paper } from "@mui/material";
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
            minWidth: 50,
            align: 'right'
        },
        { field: 'name', headerName: 'Nom', minWidth: 250 },
        {
            field: 'sport',
            headerName: 'Sport',
            minWidth: 150,
            valueGetter: (params) => {
                if (params.value) {
                    switch (params.value) {
                        case 0:
                            return "Autre"
                            break;
                        case 1:
                            return "Course \u00e0 pied"
                            break;
                        case 2:
                            return "Cyclisme"
                            break;
                        case 3:
                            return "Natation"
                            break;
                        case 4:
                            return "Randonn\u00e9e"
                            break;
                    }
                } else {
                    return "-"
                }
            }
        },
        {
            field: 'location',
            headerName: 'Lieu',
            minWidth: 170
        },
        {
            field: 'calories',
            headerName: 'Calories',
            minWidth: 70
        },
        {
            field: 'temperature',
            headerName: 'Temperature',
            minWidth: 70,
            valueGetter: (params) => {
                if (params.value || params.value === 0) {
                    return params.value + "\u00B0C"
                } else {
                    return "-"
                }
            }
        },
        {
            field: 'time',
            headerName: 'Date',
            minWidth: 170,
            valueGetter: (params) => {
                if (params.value) {
                    const dt = params.value as string
                    return dt.replace("T", " ")
                } else {
                    return "-"
                }
            }
        },
        {
            field: 'timespanTicks',
            headerName: 'Dur\u00e9e',
            minWidth: 100,
            valueGetter: (params) => {
                if (params.value) {
                    const hours = Math.floor(params.value / 10000000 / 60 / 60)
                    const minutes = Math.floor((params.value - hours * 10000000 * 60 * 60) / 10000000 / 60)
                    const seconds = Math.floor((params.value - hours * 10000000 * 60 * 60 - minutes * 10000000 * 60) / 10000000)
                    return `${hours}:${minutes}:${seconds}`
                } else {
                    return "-"
                }
            }
        }
    ];

    const { rows, loading } = useFetchActivitiesByAthlete(props.athleteId);

    const handleSelection = (row: GridRowSelectionModel) => {
        const rowId = row[0]
        console.log(`selected row ${rowId}`)
        props.onActivityIdChange(rowId as number)
    }

    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }} elevation={3}>
            <DataGrid loading={loading} rows={rows} columns={columns} onRowSelectionModelChange={handleSelection} />
        </Paper>
    );
}