import { MapContainer, TileLayer } from "react-leaflet"
import { OslMapTrack } from "./OslMapTrack"

interface OslMapProps {
    activityId: number | null;
}

export const OslMap = (props: OslMapProps) => {

    console.log("Render OslMap")

    return (
        <MapContainer center={[45.4, -75.7]} zoom={12} scrollWheelZoom={false} id="oslmap">
            <TileLayer
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
            />
            <OslMapTrack activityId={props.activityId} />
        </MapContainer >)
}