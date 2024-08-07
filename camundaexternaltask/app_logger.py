######################################################################
# File: app_logger.py
# Copyright (c) 2024 Vyacheslav Kotrachev
#
# This library is free software; you can redistribute it and/or
# modify it under the terms of the GNU Lesser General Public
# License as published by the Free Software Foundation; either
# version 2.1 of the License, or (at your option) any later version.
#
# This library is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
# Lesser General Public License for more details.
#
######################################################################
import os
import logging
from pythonjsonlogger import jsonlogger
import json
from datetime import datetime

#.%(msecs)03d
#LOG_FMT = '%(process)d %(asctime)s %(levelname)s %(message)s %(Addr)s %(TraceID)s %(Code)d %(module)s %(thread)d %(ExtUID)s %(Container)s %(Host)s %(name)s'
LOG_FMT = '%(process)d %(Time)s %(levelname)s %(message)s %(Addr)s %(TraceID)s %(Code)d %(module)s %(thread)d %(ExtUID)s %(Container)s %(Host)s %(name)s'
#'asctime'  : 'Time',
RENAMED = {
'levelname': 'Level',
'message'  : 'Text',
'module'   : 'Class',
'thread'   : 'ThreadId',
'name'     : 'Service'
}


#os.environ['NOMAD_HOST_ADDR_chronicler']='192.168.17.17:23456'

LogLevel = {}
LogLevel['DEBUG']    = logging.DEBUG
LogLevel['INFO']     = logging.INFO
LogLevel['WARNING']  = logging.WARNING
LogLevel['ERROR']    = logging.ERROR
LogLevel['CRIDummyProtocol1AL'] = logging.CRIDummyProtocol1AL

SERVICE = 'ExternalTask'
LOG_LEVEL = 'DEBUG' if 'LOG_LEVEL' not in os.environ else  os.environ['LOG_LEVEL']
LEVEL   = LogLevel[LOG_LEVEL]


class CustomJsonFormatter(jsonlogger.JsonFormatter):
  def add_fields(self, log_record, record, message_dict):
    super(CustomJsonFormatter, self).add_fields(log_record, record, message_dict)
#    if not log_record.get('timestamp'):
    if not log_record.get('Time'):
    # this doesn't use record.created, so it is slightly off
#      now = datetime.utcnow().strftime('%Y-%m-%dT%H:%M:%S.%fZ')
      now = datetime.utcnow().strftime('%Y-%m-%dT%H:%M:%S.%f0+00:00')
      log_record['Time'] = now
#      log_record['timestamp'] = now
    if log_record.get('level'):
      log_record['level'] = log_record['level'].upper()
    else:
      log_record['level'] = record.levelname

class MyFilter(logging.Filter):
  def filter(self, record):
    try:
      addr = os.environ['NOMAD_HOST_ADDR_metadata']
    except KeyError:
      addr = "None"
    record.Addr = addr
    record.Container = "MetaData"
    return True

def get_stream_handler(level):
  stream_handler = logging.StreamHandler()
  stream_handler.setLevel(level)
#  stream_handler.setFormatter(jsonlogger.JsonFormatter(LOG_FMT, rename_fields=RENAMED, json_encoder=json.JSONEncoder))
  stream_handler.setFormatter(CustomJsonFormatter(LOG_FMT, rename_fields=RENAMED, json_encoder=json.JSONEncoder))
  return stream_handler

def get_file_handler(level):
  file_handler = logging.FileHandler('server.log')
  file_handler.setLevel(level)
  file_handler.setFormatter(jsonlogger.JsonFormatter(LOG_FMT, rename_fields=RENAMED, json_encoder=json.JSONEncoder))
  return file_handler

def get_logger(unit,level):
  logger = logging.getLogger(unit)
  logger.setLevel(level)
  logger.addHandler(get_stream_handler(level))
  myfilt=MyFilter()
  logger.addFilter(myfilt)
#  logger.addHandler(get_file_handler(level))
  logger.propagate = False
  return logger

logger = get_logger(SERVICE,LEVEL)
logger.info(u'Log level = %s', LOG_LEVEL)
# log={}
# for x in ['CONSUL','ENV','GET_STREAM','GET_STREAM_COND','MAIN','MAINTENANCE','METRICS','PUT','PUT_STREAM','SQL_ROWS','CONNECT','SQL']:
#   log[x]=get_logger(x,LEVEL)
#   log[x].info(f'Log level for {x} = {LOG_LEVEL}')
