import { Grid, ThemeProvider, createTheme } from '@mui/material';
import { red } from '@mui/material/colors';
import React from 'react';
import './App.css';
import { ActivitiesList, ActivityDetails } from './features/activities';
import { OslMap } from './features/tracks';

function App() {
    const [activityId, setActivityId] = React.useState(null as number | null)
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
            <Grid container spacing={2} direction="column" sx={{ height: 'calc(100vh - 4em)', width: 'calc(100vw - 4em)' }}>
                <Grid container item spacing={3} alignItems="stretch" xs={12} sm={10} md={6} lg={6}>
                    <Grid item xs={12} lg={8}>
                        <ActivitiesList athleteId={athleteId} onActivityIdChange={handleChangeActivity}></ActivitiesList>
                    </Grid>
                    <Grid item xs={12} lg={4}>
                        <ActivityDetails activityId={activityId}></ActivityDetails>
                    </Grid>
                </Grid>
                <Grid item spacing={3} alignItems="stretch" xs >
                    <OslMap></OslMap>
                </Grid>
            </Grid>
        </ThemeProvider>
    )
}

export default App
