import { GeoJsonObject } from "geojson";
import L from "leaflet";
import { useMap } from "react-leaflet";
import useFetchTrackPointsByActivity from "../hooks/FetchTrackPointsByActivity";
import { useEffect, useState } from "react";

interface OslMapTrackProps {
    activityId: number | null;
}

export const OslMapTrack = (props: OslMapTrackProps) => {

    console.log("Render OslMapTrack")
    const map = useMap()

    const { rows } = useFetchTrackPointsByActivity(props.activityId);
    const [lineString, setLineString] = useState(L.geoJSON())

    useEffect(() => {
        if (rows.length !== 0) {
            const geoJson = {
                "type": "FeatureCollection",
                "features": [
                    {
                        "type": "Feature",
                        "geometry": {
                            "type": "LineString",
                            "coordinates":
                                rows.map(trackPoint => [trackPoint.longitude, trackPoint.latitude])
                        },
                        "properties": {
                            "zoomed": "false"
                        },
                        "id": 1
                    }
                ]
            } as GeoJsonObject;

            setLineString(L.geoJSON(geoJson));
        }
    }, [rows])

    useEffect(() => {
        if (!lineString || !map) { return }
        const geoJson = lineString.toGeoJSON();
        if (geoJson.type === "FeatureCollection" && geoJson.features.length > 0) {
            map.removeLayer(lineString)
        }
        return () => {
            const geoJson = lineString.toGeoJSON();
            if (geoJson.type === "FeatureCollection" && geoJson.features.length > 0) {
                lineString.addTo(map)
                map.fitBounds(lineString.getBounds())
            }
        }
    }, [lineString, map])

    return null
}