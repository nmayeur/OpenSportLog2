import { Grid, Paper, TextField } from "@mui/material"
import useFetchActivityById from "../hooks/FetchActivityById";
import React from "react";

interface ActivityDetailsProp {
    activityId: number | null;
}

export const ActivityDetails = (props: ActivityDetailsProp) => {

    const [baseUrl] = React.useState(`https://osl-webapiapi-dev.azure-api.net/osl-dev/api`);
    const [api_key] = React.useState("2d5915334aa74fb19fefe972c952c5d6");

    const { activity } = useFetchActivityById(baseUrl, api_key, props.activityId);

    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }} elevation={3}>
            Activity {props.activityId}
            <Grid container spacing={3}>
                <Grid item xs={6}>
                    <TextField id="athleteName" label="Nom" variant="standard" fullWidth
                        value={activity.name}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item xs={6}>
                    <TextField id="athleteLocation" label="Lieu" variant="standard" fullWidth
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>

                <Grid item xs={4}>
                    <TextField id="athleteSport" label="Sport" variant="standard" fullWidth
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item xs={1}>
                    <TextField id="athleteCalories" label="Calories" variant="standard" fullWidth
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item xs={3}>
                    <TextField id="athleteDateTime" label="Date/heure" variant="standard" fullWidth
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item xs={3}>
                    <TextField id="athleteDuration" label="Dur&eacute;e" variant="standard" fullWidth
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item xs={1}>
                    <TextField id="athleteHr" label="HR" variant="standard" fullWidth
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
            </Grid>
        </Paper>)
}