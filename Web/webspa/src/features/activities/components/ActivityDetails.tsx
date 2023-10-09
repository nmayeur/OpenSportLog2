import { Paper } from "@mui/material"

interface ActivityDetailsProp {
    activityId: number;
}

export const ActivityDetails = (props: ActivityDetailsProp) => {
    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }} elevation={3}>
            Activity {props.activityId}
        </Paper>)
}