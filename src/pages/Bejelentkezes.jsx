import { useState } from "react";
import api from "../api/axiosInstance";

function Bejelentkezes() {
  const [felhasznalonev, setFelhasznalonev] = useState("");
  const [jelszo, setJelszo] = useState("");
  const [hiba, setHiba] = useState("");

  const bejelentkezesKezeles = async (e) => {
    e.preventDefault();

    try {
      const valasz = await api.post("/Auth/login", {
        felhasznalonev,
        jelszo,
      });

      const token = valasz.data.token;
      localStorage.setItem("token", token);
      setHiba("");
      alert("Sikeres bejelentkezés!");
      // később navigáció pl. dashboard-ra
    } catch (error) {
      if (error.response) {
        setHiba(error.response.data);
      } else {
        setHiba("Ismeretlen hiba történt.");
      }
    }
  };

  return (
    <div className="p-6 max-w-sm mx-auto bg-white rounded-xl shadow-md space-y-4">
      <h1 className="text-xl font-bold">Bejelentkezés</h1>
      <form onSubmit={bejelentkezesKezeles} className="space-y-4">
        <input
          type="text"
          placeholder="Felhasználónév"
          className="border p-2 w-full"
          value={felhasznalonev}
          onChange={(e) => setFelhasznalonev(e.target.value)}
        />
        <input
          type="password"
          placeholder="Jelszó"
          className="border p-2 w-full"
          value={jelszo}
          onChange={(e) => setJelszo(e.target.value)}
        />
        <button
          type="submit"
          className="bg-blue-500 hover:bg-blue-600 text-white p-2 w-full rounded"
        >
          Bejelentkezés
        </button>
        {hiba && <p className="text-red-500">{hiba}</p>}
      </form>
    </div>
  );
}

export default Bejelentkezes;
