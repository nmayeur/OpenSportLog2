import { Container, Grid, ThemeProvider, createTheme } from '@mui/material'
import './App.css'
import { ActivitiesList, ActivityDetails } from './features/activities'
import { red } from '@mui/material/colors';

function App() {
    const theme = createTheme({
        palette: {
            primary: {
                main: red[500],
            },
        },
    });

    const handleChangeActivity = (activityId: number) => {
        console.log(`Set activity ${activityId}`)
    }

    return (
        <ThemeProvider theme={theme}>
            <Container maxWidth="xl">
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <ActivitiesList athleteId={1} onActivityIdChange={handleChangeActivity}></ActivitiesList>
                    </Grid>
                    <Grid item xs={12}>
                        <ActivityDetails activityId={2}></ActivityDetails>
                    </Grid>
                </Grid>
            </Container>
        </ThemeProvider>
    )
}

export default App
