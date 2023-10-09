import { Container, Grid, ThemeProvider, createTheme } from '@mui/material';
import { red } from '@mui/material/colors';
import React from 'react';
import './App.css';
import { ActivitiesList, ActivityDetails } from './features/activities';

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
            <Container maxWidth="xl">
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <ActivitiesList athleteId={athleteId} onActivityIdChange={handleChangeActivity}></ActivitiesList>
                    </Grid>
                    <Grid item xs={12}>
                        <ActivityDetails activityId={activityId}></ActivityDetails>
                    </Grid>
                </Grid>
            </Container>
        </ThemeProvider>
    )
}

export default App
