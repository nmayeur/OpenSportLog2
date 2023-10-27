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

    let activitySport = "-"
    switch (activity.sport) {
        case 0:
            activitySport = "Autre"
            break;
        case 1:
            activitySport = "Course \u00e0 pied"
            break;
        case 2:
            activitySport = "Cyclisme"
            break;
        case 3:
            activitySport = "Natation"
            break;
        case 4:
            activitySport = "Randonn\u00e9e"
            break;
    }

    let activityTemperature = "-"
    if (activity.temperature || activity.temperature === 0) {
        activityTemperature = `${activity.temperature}\u00B0C`
    }

    let activityDuration = "-"
    if (activity.timeSpanTicks) {
        const hours = Math.floor(activity.timeSpanTicks / 10000000 / 60 / 60)
        const minutes = Math.floor((activity.timeSpanTicks - hours * 10000000 * 60 * 60) / 10000000 / 60)
        const seconds = Math.floor((activity.timeSpanTicks - hours * 10000000 * 60 * 60 - minutes * 10000000 * 60) / 10000000)
        activityDuration = `${hours}:${minutes}:${seconds}`
    }

    let activityTime = ""
    if (activity.time) {
        activityTime = `${activity.time?.toLocaleDateString()} ${activity.time?.toLocaleTimeString()}`
    }

    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }} elevation={3}>
            <Grid container spacing={1}>
                <Grid item md={12}>
                    <TextField id="athleteName" label="Nom" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activity.name}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={12}>
                    <TextField id="athleteLocation" label="Lieu" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activity.location}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>

                <Grid item md={6}>
                    <TextField id="athleteSport" label="Sport" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activitySport}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={6}>
                    <TextField id="athleteCalories" label="Calories" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activity.calories}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={6}>
                    <TextField id="athleteDateTime" label="Date/heure" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activityTime}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={6}>
                    <TextField id="athleteDuration" label="Dur&eacute;e" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activityDuration}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={3}>
                    <TextField id="athleteHr" label="HR" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activity.heartRate}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={3}>
                    <TextField id="temperature" label="Temp&eacute;rature" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activityTemperature}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={6}>
                    <TextField id="originSystem" label="Syst&egrave;me d'origine" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activity.originSystem}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={3}>
                    <TextField id="cadence" label="Cadence" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activity.cadence}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
                <Grid item md={3}>
                    <TextField id="power" label="Puissance" variant="standard" fullWidth
                        InputLabelProps={{ shrink: true }}
                        value={activity.power}
                        InputProps={{
                            readOnly: true,
                        }} />
                </Grid>
            </Grid>
        </Paper>)
}