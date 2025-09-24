import * as signalR from '@microsoft/signalr';
import type { MessageDTO } from '@/types/dtos';

export const chatConnection = new signalR.HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_API_BASE.replace('/api','')}/hub/chat`, {
    withCredentials: true 
  })
  .withAutomaticReconnect()
  .build();

export const onMessageSended = (callback: (msg: MessageDTO) => void) => {
  chatConnection.on('MessageSended', callback);
};

export const onMessageUpdated = (callback: (msg: MessageDTO) => void) => {
  chatConnection.on('MessageUpdated', callback);
};

export const startChatHub = async () => {
  try {
    await chatConnection.start();
    console.log('SignalR ChatHub connected');
  } catch (err) {
    console.error('SignalR connection error', err);
  }
};
