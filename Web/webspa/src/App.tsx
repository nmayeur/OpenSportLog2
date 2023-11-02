import { Grid, Stack, ThemeProvider, createTheme } from '@mui/material';
import { red } from '@mui/material/colors';
import { useState } from 'react';
import './App.css';
import { ActivitiesList, ActivityDetails } from './features/activities';
import { OslMap } from './features/tracks';
import { OslAppBar } from './features/appBar';

function App() {
    const [activityId, setActivityId] = useState(null as (number | null))
    const athleteId = 1

    const theme = createTheme({
        palette: {
            primary: {
                main: red[500],
            },
        },
    });

    const handleChangeActivity = (activityId: number) => {
        console.log(`Set activity ${activityId}`)
        setActivityId(activityId)
    }

    return (
        <ThemeProvider theme={theme}>
            <Stack sx={{ height: 'calc(100vh - 4em)', width: 'calc(100vw - 4em)' }} spacing={2}>
                <OslAppBar></OslAppBar>
                <Grid container item spacing={3} alignItems="stretch" xs={12} sm={10} md={6} lg={6}>
                    <Grid item xs={12} lg={8}>
                        <ActivitiesList athleteId={athleteId} onActivityIdChange={handleChangeActivity}></ActivitiesList>
                    </Grid>
                    <Grid item xs={12} lg={4}>
                        <ActivityDetails activityId={activityId}></ActivityDetails>
                    </Grid>
                </Grid>
                <OslMap activityId={activityId}></OslMap>
            </Stack>
        </ThemeProvider>
    )
}

export default App
