// src/api/wellbeingService.js
import api from './api';

export const createEntry = async (payload) => {
  const res = await api.post('/wellbeing', payload);
  return res.data;
};

export const getEntries = async () => {
  const res = await api.get('/wellbeing');
  return res.data;
};

export const updateEntry = async (id, payload) => {
  const res = await api.put(`/wellbeing/${id}`, payload);
  return res.data;
};

export const deleteEntry = async (id) => {
  const res = await api.delete(`/wellbeing/${id}`);
  return res.data;
};

export const detectEmotion = async (formData) => {
  const res = await api.post('/api/emocao', formData, { headers: { 'Content-Type': 'multipart/form-data' } });
  return res.data;
};

export const getLastIoT = async () => {
  const res = await api.get('/api/iot/ultimo');
  return res.data;
};
