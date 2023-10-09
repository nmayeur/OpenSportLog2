import { Grid, Paper, TextField } from "@mui/material"

interface ActivityDetailsProp {
    activityId: number | null;
}

export const ActivityDetails = (props: ActivityDetailsProp) => {
    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }} elevation={3}>
            Activity {props.activityId}
            <Grid container spacing={3}>
                <Grid item xs={6}>
                    <TextField id="athleteName" label="Nom" variant="standard" fullWidth
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