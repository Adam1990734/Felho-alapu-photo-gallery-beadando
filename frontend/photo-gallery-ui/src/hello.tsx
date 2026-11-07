import { useEffect, useState } from "react";

export default function Hello() {
    const [hello, setHello] = useState<string>("");

    const getHello = async () => {
        const result = await fetch("/api/sayhello").then(res => res.json());
        setHello(result.message);
    };

    useEffect(() => {
        getHello();
    }, []);

    return (<>
        <h1>{hello ? hello : "Betöltés alatt..."}</h1>
    </>);
}