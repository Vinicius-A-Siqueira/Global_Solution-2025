import { useEffect, useState } from 'react';
import { Button, Text, View } from 'react-native';
import { getLastIoT } from '../api/wellbeingService';

export default function EnvironmentScreen() {
  const [data, setData] = useState(null);
  const load = async () => {
    try {
      const res = await getLastIoT();
      setData(res);
    } catch (e) { console.error(e); }
  };
  useEffect(()=>{ load(); },[]);
  return (
    <View style={{flex:1,padding:16}}>
      <Button title="Atualizar" onPress={load} />
      {data ? (
        <>
          <Text>Temperatura: {data.temperatura}</Text>
          <Text>Umidade: {data.umidade}</Text>
          <Text>Luminosidade: {data.luminosidade}</Text>
          <Text>Ruído: {data.ruido}</Text>
        </>
      ) : <Text>Nenhum dado</Text>}
    </View>
  );
}
