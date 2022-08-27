# coding: utf-8

from __future__ import absolute_import
from datetime import date, datetime  # noqa: F401

from typing import List, Dict  # noqa: F401

from openapi_server.models.base_model_ import Model
from openapi_server import util


class InlineObject2(Model):
    """NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).

    Do not edit the class manually.
    """

    def __init__(self, username=None, password=None):  # noqa: E501
        """InlineObject2 - a model defined in OpenAPI

        :param username: The username of this InlineObject2.  # noqa: E501
        :type username: str
        :param password: The password of this InlineObject2.  # noqa: E501
        :type password: str
        """
        self.openapi_types = {
            'username': str,
            'password': str
        }

        self.attribute_map = {
            'username': 'username',
            'password': 'password'
        }

        self._username = username
        self._password = password

    @classmethod
    def from_dict(cls, dikt) -> 'InlineObject2':
        """Returns the dict as a model

        :param dikt: A dict.
        :type: dict
        :return: The inline_object_2 of this InlineObject2.  # noqa: E501
        :rtype: InlineObject2
        """
        return util.deserialize_model(dikt, cls)

    @property
    def username(self):
        """Gets the username of this InlineObject2.


        :return: The username of this InlineObject2.
        :rtype: str
        """
        return self._username

    @username.setter
    def username(self, username):
        """Sets the username of this InlineObject2.


        :param username: The username of this InlineObject2.
        :type username: str
        """

        self._username = username

    @property
    def password(self):
        """Gets the password of this InlineObject2.


        :return: The password of this InlineObject2.
        :rtype: str
        """
        return self._password

    @password.setter
    def password(self, password):
        """Sets the password of this InlineObject2.


        :param password: The password of this InlineObject2.
        :type password: str
        """

        self._password = password
