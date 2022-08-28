import connexion
import six

from openapi_server.models.error import Error  # noqa: E501
from openapi_server.models.inline_object import InlineObject  # noqa: E501
from openapi_server.models.user import User  # noqa: E501
from openapi_server import util


def login(inline_object):  # noqa: E501
    """Log in

    Attempts to log a user in # noqa: E501

    :param inline_object: 
    :type inline_object: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        inline_object = InlineObject.from_dict(connexion.request.get_json())  # noqa: E501
    return 'do some magic!'


def register(user=None):  # noqa: E501
    """Registers User

    Attempts to register a new user # noqa: E501

    :param user: 
    :type user: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        user = User.from_dict(connexion.request.get_json())  # noqa: E501
    return 'do some magic!'
